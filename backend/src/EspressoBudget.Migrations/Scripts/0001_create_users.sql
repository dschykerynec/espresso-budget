CREATE TABLE users (
    id uuid PRIMARY KEY DEFAULT uuidv7(),
    username text NOT NULL UNIQUE,
    created_at timestamptz NOT NULL DEFAULT now()
);