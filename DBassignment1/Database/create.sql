PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS Book (
    book_id INTEGER PRIMARY KEY AUTOINCREMENT,
    title TEXT NOT NULL,
    author TEXT NOT NULL,
    publication_year INTEGER CHECK (publication_year > 0),
    isbn TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Borrower (
    borrower_id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    phone TEXT NOT NULL,
    email TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Loan (
    loan_id INTEGER PRIMARY KEY AUTOINCREMENT,
    book_id INTEGER NOT NULL,
    borrower_id INTEGER NOT NULL,
    loan_date DATE NOT NULL,
    due_date DATE NOT NULL CHECK (due_date > loan_date),
    return_date DATE NULL,
    FOREIGN KEY (book_id) REFERENCES Book(book_id) ON DELETE CASCADE,
    FOREIGN KEY (borrower_id) REFERENCES Borrower(borrower_id) ON DELETE CASCADE
);
