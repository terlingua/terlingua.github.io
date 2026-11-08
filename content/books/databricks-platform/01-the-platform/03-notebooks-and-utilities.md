---
title: Notebooks and Databricks utilities
questions:
  - id: magic
    prompt: In a Python notebook, how do you run one cell as SQL?
    options:
      - text: Rename the notebook to `.sql`
      - text: Use `spark.sqlcell()`
      - text: It is not possible to mix languages
      - id: magic-sql
        text: Start the cell with the `%sql` magic command
        correct: true
    explanation: Magic commands such as `%sql`, `%python`, `%scala`, `%r`, `%md`, `%sh`, `%pip` and `%run` change a cell's behaviour.
  - id: secrets
    prompt: What is the right way to use a database password in a notebook?
    options:
      - text: Paste it into a cell and clear the output afterwards
      - id: secret-scope
        text: Read it with `dbutils.secrets.get(scope, key)`; values are redacted if printed in notebook output
        correct: true
      - text: Store it in a widget's default value
    explanation: Secret scopes keep credentials out of code and notebook history. Better still, use Unity Catalog connections or identity-based access where possible.
  - id: dbutils
    prompt: Which are capabilities of `dbutils`?
    type: multiple
    options:
      - text: "`dbutils.fs` — file system operations such as listing and copying files"
        correct: true
      - text: "`dbutils.widgets` — notebook parameters"
        correct: true
      - text: "`dbutils.secrets` — reading secrets from secret scopes"
        correct: true
      - text: "`dbutils.compile` — compiling Scala to native code"
    explanation: "`dbutils` also includes `notebook` (run other notebooks) and `jobs.taskValues` (pass values between job tasks)."
---
## Learning objectives

- Work effectively in Databricks notebooks
- Use magic commands and `dbutils`
- Handle parameters and secrets safely

## Notebooks

Notebooks combine code, results, visualisations and Markdown. A notebook has a default language
(Python, SQL, Scala or R), and individual cells can switch with magic commands:

```python
df = spark.read.table("samples.nyctaxi.trips")
display(df.limit(10))
```

```sql
%sql
SELECT pickup_zip, COUNT(*) AS trips
FROM samples.nyctaxi.trips
GROUP BY pickup_zip
ORDER BY trips DESC
LIMIT 10
```

`spark` (a `SparkSession`) and `display()` are available automatically.

## Magic commands

| Command | Purpose |
|---------|---------|
| `%sql`, `%python`, `%scala`, `%r` | Change cell language |
| `%md` | Markdown documentation |
| `%pip install pkg` | Install a notebook-scoped Python library |
| `%run ./helpers` | Run another notebook inline |
| `%sh` | Shell commands on the driver |

## Widgets as parameters

```python
dbutils.widgets.text("run_date", "2026-11-01", "Run date")
run_date = dbutils.widgets.get("run_date")

orders = spark.read.table("prod.sales.orders").where(f"order_date = '{run_date}'")
```

When the notebook runs as a job task, job parameters populate the widgets. (For SQL, prefer
parameter markers such as `:run_date` over string formatting.)

## Secrets

```python
password = dbutils.secrets.get(scope="erp", key="jdbc-password")
print(password)   # shows [REDACTED]
```

## Files

```python
display(dbutils.fs.ls("/Volumes/main/raw/landing/"))
```

Unity Catalog **volumes** (`/Volumes/<catalog>/<schema>/<volume>/...`) are the governed place for
non-tabular files. The legacy DBFS root is discouraged for new work.

## Beyond notebooks

Notebooks are great for exploration. For production code, move logic into Python modules or
packages in Git folders, add unit tests, and call them from thin notebooks or Python script tasks.
