-- 003_InsertSampleData.sql

-- Insert sample persons (only if they don't already exist)
INSERT INTO public.person (first_name, last_name)
SELECT 'John', 'Doe'
WHERE NOT EXISTS (SELECT 1 FROM public.person WHERE first_name = 'John' AND last_name = 'Doe');

INSERT INTO public.person (first_name, last_name)
SELECT 'Jane', 'Smith'
WHERE NOT EXISTS (SELECT 1 FROM public.person WHERE first_name = 'Jane' AND last_name = 'Smith');

INSERT INTO public.person (first_name, last_name)
SELECT 'Bob', 'Johnson'
WHERE NOT EXISTS (SELECT 1 FROM public.person WHERE first_name = 'Bob' AND last_name = 'Johnson');

INSERT INTO public.person (first_name, last_name)
SELECT 'Alice', 'Williams'
WHERE NOT EXISTS (SELECT 1 FROM public.person WHERE first_name = 'Alice' AND last_name = 'Williams');

INSERT INTO public.person (first_name, last_name)
SELECT 'Charlie', 'Brown'
WHERE NOT EXISTS (SELECT 1 FROM public.person WHERE first_name = 'Charlie' AND last_name = 'Brown');

-- Insert sample import batches (only if they don't already exist)
INSERT INTO public.import_batch (name, status)
SELECT 'Initial Import', 'completed'
WHERE NOT EXISTS (SELECT 1 FROM public.import_batch WHERE name = 'Initial Import');

INSERT INTO public.import_batch (name, status)
SELECT 'Monthly Update', 'pending'
WHERE NOT EXISTS (SELECT 1 FROM public.import_batch WHERE name = 'Monthly Update');

INSERT INTO public.import_batch (name, status)
SELECT 'Data Sync', 'completed'
WHERE NOT EXISTS (SELECT 1 FROM public.import_batch WHERE name = 'Data Sync');

