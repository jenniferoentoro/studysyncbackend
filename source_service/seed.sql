CREATE TABLE category
(
    id serial PRIMARY KEY,
    name VARCHAR (100) NOT NULL
);

INSERT INTO category (name) VALUES
    ('Category 1'),
    ('Category 2'),
    ('Category 3'),
    ('Category 4');