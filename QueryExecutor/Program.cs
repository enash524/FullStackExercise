using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Priority_Queue;
using QueryExecutor.Models;

namespace QueryExecutor
{
    internal class Program
    {
        #region Private Fields

        private static bool processing = false;
        private static SimplePriorityQueue<Query> queue = new SimplePriorityQueue<Query>();

        #endregion Private Fields

        #region Private Methods

        private static void Main(string[] args)
        {
            string dataDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\App_Data";
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);

            Schedule(new Query
            {
                Parameters = new List<SqlParameter>
                {
                    new SqlParameter("@firstName", "First"),
                    new SqlParameter("@lastName", "User1")
                },
                Priority = QueryPriority.High,
                Statement = "uspInsertUserInfoRecord"
            });

            Schedule(new Query
            {
                Parameters = new List<SqlParameter>
                {
                    new SqlParameter("@firstName", "Second"),
                    new SqlParameter("@lastName", "User2")
                },
                Priority = QueryPriority.Medium,
                Statement = "uspInsertUserInfoRecord"
            });

            Schedule(new Query
            {
                Parameters = new List<SqlParameter>
                {
                    new SqlParameter("@firstName", "Third"),
                    new SqlParameter("@lastName", "User3")
                },
                Priority = QueryPriority.Low,
                Statement = "uspInsertUserInfoRecord"
            });

            Schedule(new Query
            {
                Parameters = new List<SqlParameter>
                {
                    new SqlParameter("@firstName", "Fourth"),
                    new SqlParameter("@lastName", "User4")
                },
                Priority = QueryPriority.High,
                Statement = "uspInsertUserInfoRecord"
            });

            Start();
        }

        private static void ProcessQuery(Query query)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = query.Statement;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(query.Parameters.ToArray());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void Schedule(Query query)
        {
            queue.Enqueue(query, (float)query.Priority);
        }

        private static void Start()
        {
            processing = true;

            while (processing && queue.Count > 0)
            {
                Query query = queue.Dequeue();
                ProcessQuery(query);
            }

            processing = false;
        }

        private static void Stop()
        {
            processing = false;
        }

        #endregion Private Methods
    }
}