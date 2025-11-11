-- CheckData.sql - Queries to verify inserted data

-- First, check what tables exist
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public'
ORDER BY table_name;

-- Create import_batch table if it doesn't exist
CREATE TABLE IF NOT EXISTS public.import_batch (
    id serial PRIMARY KEY,
    name text NOT NULL,
    status text NOT NULL,
    created_at timestamptz DEFAULT now()
);

-- Check all persons
SELECT * FROM public.person;

-- Check all import batches
SELECT * FROM public.import_batch;

-- Count records
SELECT 
    (SELECT COUNT(*) FROM public.person) as person_count,
    (SELECT COUNT(*) FROM public.import_batch) as import_batch_count;

