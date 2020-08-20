namespace SharedClassLibrary.Simulation
{
    public abstract class ComponentRollbackState
    {
        private ComponentRollbackState updatedState = null;

        /// <summary>
        /// References self if no changes have been made. References updated state if changes have been made.
        /// </summary>
        protected ComponentRollbackState activeState
        {
            get
            {
                if (updatedState == null)
                {
                    return this;
                }
                else
                {
                    return updatedState;
                }
            }
        }

        /// <summary>
        /// Creates a copy of the current ComponentRollbackState. If not overridden, a shallow copy.
        /// </summary>
        /// <returns>
        /// A copy of the current ComponentRollbackState;
        /// </returns>
        protected virtual ComponentRollbackState Clone()
        {
            return this.MemberwiseClone() as ComponentRollbackState;
        }

        /// <summary>
        /// Clones current ComponentRollbackState assignes it to updated state if updated state is null. Call when a change is about to be made to this object.
        /// </summary>
        protected void ChangeMade()
        {
            if (updatedState == null)
            {
                updatedState = Clone();
            }
        }

        internal ComponentRollbackState FinishChanges()
        {
            ComponentRollbackState ret = activeState;
            updatedState = null;
            return ret;
        }
    }
}
