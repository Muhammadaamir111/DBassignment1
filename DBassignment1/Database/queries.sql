-- Retrieve all books
SELECT * FROM Book;

-- Retrieve all borrowers
SELECT * FROM Borrower;

-- Retrieve all loans with book and borrower details
SELECT Loan.loan_id, Book.title, Borrower.name, Loan.loan_date, Loan.due_date, Loan.return_date
FROM Loan
JOIN Book ON Loan.book_id = Book.book_id
JOIN Borrower ON Loan.borrower_id = Borrower.borrower_id;
