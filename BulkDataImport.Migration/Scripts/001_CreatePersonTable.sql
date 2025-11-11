-- 001_CreatePersonTable.sql

CREATE TABLE IF NOT EXISTS public.person (
    id serial PRIMARY KEY,
    first_name text NOT NULL,
    last_name text NOT NULL,
    created_at timestamptz DEFAULT now()
);

