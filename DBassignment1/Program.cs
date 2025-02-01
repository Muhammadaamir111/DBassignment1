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

            // **Delete the existing database file on every run**
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
                Console.WriteLine("Old database file deleted.");
            }

            // Create a new SQLite database file
            SQLiteConnection.CreateFile(dbPath);
            Console.WriteLine("New database file created.");

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

                // Execute SQL scripts
                ExecuteSqlScript(conn, createSqlPath);
                ExecuteSqlScript(conn, populateSqlPath);
                ExecuteSqlScript(conn, queriesSqlPath);

                Console.WriteLine("Database setup complete.\n");

                // Fetch and display loaned books
                DisplayLoanedBooks(conn);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads and executes an SQL script file.
    /// Prevents duplicate inserts in `populate.sql`.
    /// </summary>
    static void ExecuteSqlScript(SQLiteConnection conn, string scriptPath)
    {
        try
        {
            if (!File.Exists(scriptPath))
            {
                Console.WriteLine($"SQL file not found: {scriptPath}");
                return;
            }

            string sql = File.ReadAllText(scriptPath);

            // Prevent duplicate inserts by checking if data exists
            if (scriptPath.Contains("populate.sql"))
            {
                using (var checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Book;", conn))
                {
                    int bookCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (bookCount > 0)
                    {
                        Console.WriteLine("Skipping populate.sql: Data already exists.");
                        return;
                    }
                }
            }

            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine($"Executed script: {scriptPath}");
        }
        catch (SQLiteException ex)
        {
            Console.WriteLine($"Error executing script {scriptPath}: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves and displays all loaned books with borrower details.
    /// </summary>
    static void DisplayLoanedBooks(SQLiteConnection conn)
    {
        try
        {
            string query = @"
                SELECT 
                    Loan.loan_id, 
                    Book.title, 
                    Borrower.name, 
                    Loan.loan_date, 
                    Loan.due_date, 
                    COALESCE(Loan.return_date, 'NULL') AS return_date
                FROM Loan
                JOIN Book ON Loan.book_id = Book.book_id
                JOIN Borrower ON Loan.borrower_id = Borrower.borrower_id;";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("--- Loaned Books ---");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Loan ID: {reader["loan_id"]}, Book: {reader["title"]}, Borrower: {reader["name"]}, " +
                                          $"Loan Date: {Convert.ToDateTime(reader["loan_date"]).ToString("yyyy-MM-dd")}, " +
                                          $"Due Date: {Convert.ToDateTime(reader["due_date"]).ToString("yyyy-MM-dd")}, " +
                                          $"Return Date: {reader["return_date"]}");
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
