using System;

namespace GameServer.GUI.ConsoleCommands
{
    class CreateEmptyGameObject : BaseConsoleCommand
    {
        public CreateEmptyGameObject()
        {
            commandWord = "create_teste_object";
            description = "Creates a new test object.";
            peramDescription = "(float x_position, float y_position)";
        }
        protected override string Process(string[] _perams)
        {
            if (_perams.Length != 2)
            {
                return ThrowError($"Expected 2 peramiters, found {_perams.Length}.");
            }

            float _x;
            float _y;

            try
            {
                _x = float.Parse(_perams[0]);
                _y = float.Parse(_perams[1]);
            }
            catch(Exception _ex)
            {
                return ThrowError(_ex.Message);
            }

            new TestObject(new Vector2(_x,_y));

            return $"Object Created at x: {_perams[0]}, y: {_perams[1]}.";
        }
    }
}