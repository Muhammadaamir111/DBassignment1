using System;
using System.Data.SQLite;
using System.IO;

class Program
{
    static string dbPath = "library.db";

    static void Main()
    {
        try
        {
            Console.WriteLine("Starting the Library System...");

            // Ensure database file exists
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
                Console.WriteLine("Database file created.");
            }

            using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
            {
                conn.Open();
                Console.WriteLine("Connected to SQLite database.");

                // Define paths for SQL files
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string dbFolder = Path.Combine(basePath, "Database");

                string createSqlPath = Path.Combine(dbFolder, "create.sql");
                string populateSqlPath = Path.Combine(dbFolder, "populate.sql");
                string queriesSqlPath = Path.Combine(dbFolder, "queries.sql");

                // Ensure the Database folder exists
                if (!Directory.Exists(dbFolder))
                {
                    Console.WriteLine($"Database folder not found: {dbFolder}");
                    return;
                }

                // Check if SQL files exist before execution
                if (!File.Exists(createSqlPath))
                {
                    Console.WriteLine($"SQL file not found: {createSqlPath}");
                    return;
                }
                if (!File.Exists(populateSqlPath))
                {
                    Console.WriteLine($"SQL file not found: {populateSqlPath}");
                    return;
                }

                // Execute SQL scripts
                ExecuteSqlScript(conn, createSqlPath);
                ExecuteSqlScript(conn, populateSqlPath);

                Console.WriteLine("Database setup complete.");

                // Run queries to fetch data
                ExecuteQuery(conn);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads and executes an SQL script file.
    /// </summary>
    static void ExecuteSqlScript(SQLiteConnection conn, string scriptPath)
    {
        try
        {
            string sql = File.ReadAllText(scriptPath);
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine($"Executed script: {scriptPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing script {scriptPath}: {ex.Message}");
        }
    }

    /// <summary>
    /// Executes a sample query and prints the results.
    /// </summary>
    static void ExecuteQuery(SQLiteConnection conn)
    {
        try
        {
            string query = "SELECT Loan.loan_id, Book.title, Borrower.name, Loan.loan_date, Loan.due_date, Loan.return_date " +
                           "FROM Loan " +
                           "JOIN Book ON Loan.book_id = Book.book_id " +
                           "JOIN Borrower ON Loan.borrower_id = Borrower.borrower_id;";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\n--- Loaned Books ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Loan ID: {reader["loan_id"]}, Book: {reader["title"]}, Borrower: {reader["name"]}, Loan Date: {reader["loan_date"]}, Due Date: {reader["due_date"]}, Return Date: {reader["return_date"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing query: {ex.Message}");
        }
    }
}
