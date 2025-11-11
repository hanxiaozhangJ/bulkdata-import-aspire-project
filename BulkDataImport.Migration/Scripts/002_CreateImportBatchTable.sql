-- 002_CreateImportBatchTable.sql
CREATE TABLE IF NOT EXISTS public.import_batch (
    id serial PRIMARY KEY,
    name text NOT NULL,
    status text NOT NULL,
    created_at timestamptz DEFAULT now()
);

