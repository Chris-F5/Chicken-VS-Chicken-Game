using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedClassLibrary.ECS;

namespace TestProject
{
    [TestClass]
    public class ECSTest
    {
        ComponentManager<TestComponent> componentManager = new ComponentManager<TestComponent>();

        [TestMethod]
        public void Test()
        {
            World world = new World();
            EntityHandler entityA = world.CreateEntity();
            entityA.AddComponent(new TestComponent());

            EntityHandler entityB = world.CreateEntity();
            entityB.AddComponent(new TestComponent());

            GameSystem system = new TestSystem(world);

            Assert.IsTrue(entityA.GetComponent<TestComponent>().foo == 0, "component default value is wrong");

            system.Update();
            Assert.IsTrue(entityA.GetComponent<TestComponent>().foo == 1, "component A did not update correctly after system update");
            Assert.IsTrue(entityB.GetComponent<TestComponent>().foo == 1, "component B did not update correctly after system update");

            system.Update();
            Assert.IsTrue(entityA.GetComponent<TestComponent>().foo == 2, "component A did not update correctly after system update");
            Assert.IsTrue(entityB.GetComponent<TestComponent>().foo == 2, "component B did not update correctly after system update");

            world.RemoveEntity(entityA.entity);
            Assert.IsTrue(
                !world.GetComponentManager<TestComponent>().EntityHasComponent(entityA.entity),
                "entity A was not removed");

            system.Update();
            Assert.IsTrue(entityB.GetComponent<TestComponent>().foo == 3, "component B did not update correctly after system update");
        }

        #region EventBusTest

        [TestMethod]
        public void EventBusTest()
        {
            EventBus eventBus = new EventBus();
            eventBus.Subscribe<TestClassA>(TestHandlerA);
            eventBus.Subscribe<TestClassB>(TestHandlerB);

            eventBus.Publish(new TestClassA());
            Assert.IsTrue(testACalled, "testA not called");
            Assert.IsTrue(!testBCalled, "testB should not be called yet");

            eventBus.Publish(new TestClassB());
            Assert.IsTrue(testBCalled, "testB not called");
        }
        bool testACalled = false;
        bool testBCalled = false;
        public void TestHandlerA(TestClassA _test)
        {
            testACalled = true;
        }
        public void TestHandlerB(TestClassB _test)
        {
            testBCalled = true;
        }
        #endregion EventBusTest
    }

    public class TestClassA
    {
        int foo = 5;
    }
    public class TestClassB
    {
        int foo = 5;
    }

    struct TestComponent
    {
        public int foo;
    }

    class TestSystem : GameSystem
    {
        private ComponentManager<TestComponent> testComponentManager;

        public TestSystem(World _world) : base(_world, typeof(TestComponent))
        {
            componentMask.GetComponentManager(out testComponentManager);
        }

        public override void Update()
        {
            foreach (Entity entity in activeEntities)
            {
                ref TestComponent testComponent = ref testComponentManager.GetComponent(entity);
                testComponent.foo += 1;
            }
        }
    }
}
