using System;

namespace Vampwolf.Grid
{
    public class GridPredicate
    {
        private readonly Func<GridCell, bool> func;

        public GridPredicate(Func<GridCell, bool> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Evaluate the predicate
        /// </summary>
        public bool Evaluate(GridCell data) => func.Invoke(data);
    }
}
