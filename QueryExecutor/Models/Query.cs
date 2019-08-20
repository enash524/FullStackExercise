using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QueryExecutor.Models
{
    public class Query : IComparable<Query>, IComparer<Query>
    {
        #region Public Properties

        public List<SqlParameter> Parameters { get; set; } = new List<SqlParameter>();

        public QueryPriority Priority { get; set; }

        public string Statement { get; set; }

        #endregion Public Properties

        #region Public Methods

        public int Compare(Query x, Query y)
        {
            if (x.Priority == y.Priority)
                return 0;

            if ((int)x.Priority < (int)y.Priority)
                return -1;

            return 1;
        }

        public int CompareTo(Query other)
        {
            return Compare(this, other);
        }

        #endregion Public Methods
    }
}