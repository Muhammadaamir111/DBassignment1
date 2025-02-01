## ER diagram

```mermaid
erDiagram
    BOOK {
        INTEGER book_id PK "Primary Key, Auto Increment"
        VARCHAR title "NOT NULL"
        VARCHAR author "NOT NULL"
        INTEGER publication_year "CHECK (publication_year > 0)"
        VARCHAR isbn "NOT NULL, UNIQUE"
    }

    BORROWER {
        INTEGER borrower_id PK "Primary Key, Auto Increment"
        VARCHAR name "NOT NULL"
        VARCHAR phone "NOT NULL"
        VARCHAR email "NOT NULL, UNIQUE"
    }

    LOAN {
        INTEGER loan_id PK "Primary Key, Auto Increment"
        INTEGER book_id FK "Foreign Key to Book"
        INTEGER borrower_id FK "Foreign Key to Borrower"
        DATE loan_date "NOT NULL"
        DATE due_date "NOT NULL, CHECK (due_date > loan_date)"
        DATE return_date "NULL"
    }

    BOOK ||--o{ LOAN : "loaned out"
    BORROWER ||--o{ LOAN : "borrows"
```
-----------------------------------------------------


## UML diagram
```mermaid
classDiagram
    class Book {
        +Integer book_id
        +String title
        +String author
        +int publication_year "CHECK (publication_year > 0)"
        +String isbn "CHECK (LENGTH(isbn) BETWEEN 10 AND 13)"
    }

    class Borrower {
        +Integer borrower_id
        +String name
        +String phone "CHECK (LENGTH(phone) BETWEEN 7 AND 15)"
        +String email
    }

    class Loan {
        +Integer loan_id
        +Integer book_id
        +Integer borrower_id
        +Date loan_date
        +Date due_date "CHECK (due_date > loan_date)"
        +Date return_date
    }

    Book "1" --o "0..*" Loan : "loaned out"
    Borrower "1" --o "0..*" Loan : "borrows"

   ```
-----------------------------------------------------
## SQL code
    
### Insert data
INSERT INTO Book (title, author, publication_year, isbn) VALUES ('The Hobbit', 'J.R.R. Tolkien', 1937, '9780345339683');
INSERT INTO Borrower (name, phone, email) VALUES ('Anna Svensson', '0701234567', 'anna.svensson@mail.com');
INSERT INTO Loan (book_id, borrower_id, loan_date, due_date) VALUES (1, 1, '2024-01-01', '2024-01-15');

### Select data
SELECT * FROM Book;
SELECT * FROM Borrower;
SELECT * FROM Loan;

### Update data
UPDATE Book SET title = 'The Hobbit: Illustrated Edition' WHERE book_id = 1;
UPDATE Loan SET return_date = '2024-01-14' WHERE loan_id = 1;

### Delete data
DELETE FROM Loan WHERE loan_id = 1;
DELETE FROM Borrower WHERE borrower_id = 1;
DELETE FROM Book WHERE book_id = 1;

### Implementing views
CREATE VIEW Borrower_Loans AS
SELECT Borrower.name, Borrower.email, Book.title, Loan.loan_date, Loan.due_date, Loan.return_date
FROM Borrower
JOIN Loan ON Borrower.borrower_id = Loan.borrower_id
JOIN Book ON Loan.book_id = Book.book_id;

CREATE VIEW Overdue_Loans AS
SELECT Book.title, Borrower.name, Loan.due_date
FROM Loan
JOIN Book ON Loan.book_id = Book.book_id
JOIN Borrower ON Loan.borrower_id = Borrower.borrower_id
WHERE Loan.return_date IS NULL AND Loan.due_date < DATE('now');


## Security Considerations

To implement a secure login function for the library system, users will enter their credentials in a login form. The system will retrieve the stored hashed password from the database and hash the input password for comparison. If both hashed values match, the user is authenticated successfully and granted access. A strong password policy should be enforced, requiring at least 8 characters with a combination of numbers and special symbols. Passwords must be securely stored using hashing algorithms such as bcrypt. To prevent SQL injection attacks, all database queries should be executed using prepared statements, ensuring that user input is treated strictly as data. Additionally, session management should be properly handled to maintain authentication states, and failed login attempts should be logged to detect any unusual behavior or brute-force attempts.

Prepared statements are essential for preventing SQL injection attacks. Instead of embedding user input directly into SQL queries, placeholders are used, which ensures that user input is not interpreted as executable code. This technique separates the SQL logic from the input data, making it impossible for an attacker to manipulate queries.

