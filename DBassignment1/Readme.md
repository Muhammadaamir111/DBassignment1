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
--------------------------------------------------------
## UML diagram

```mermaid class diagram
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
----------
