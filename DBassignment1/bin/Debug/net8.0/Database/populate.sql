INSERT INTO Book (title, author, publication_year, isbn) VALUES
    ('The Hobbit', 'J.R.R. Tolkien', 1937, '9780345339683'),
    ('1984', 'George Orwell', 1949, '9780451524935');

INSERT INTO Borrower (name, phone, email) VALUES
    ('Anna Svensson', '0701234567', 'anna.svensson@mail.com'),
    ('Erik Johansson', '0709876543', 'erik.johansson@mail.com');

INSERT INTO Loan (book_id, borrower_id, loan_date, due_date, return_date) VALUES
    (1, 1, '2024-01-01', '2024-01-15', NULL),
    (2, 2, '2024-01-05', '2024-01-20', '2024-01-18');
