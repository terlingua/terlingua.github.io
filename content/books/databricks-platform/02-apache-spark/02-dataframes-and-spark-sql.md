---
title: DataFrames and Spark SQL
questions:
  - id: same-engine
    prompt: How do the DataFrame API and Spark SQL relate in terms of performance?
    options:
      - text: SQL is always much faster than DataFrames
      - text: DataFrames are always much faster than SQL
      - id: same
        text: Both compile to the same logical plan and are optimised by Catalyst, so equivalent queries perform the same
        correct: true
    explanation: Choose whichever is clearer for the task. You can mix them freely, for example with `spark.sql` and temporary views.
  - id: udf
    prompt: Why should built-in functions be preferred over Python UDFs where possible?
    options:
      - text: Python UDFs are not allowed on Databricks
      - id: optimise
        text: Built-in functions run inside the engine (and Photon) and can be optimised, while Python UDFs require serializing data to Python workers
        correct: true
      - text: Built-in functions support more data types than Python
    explanation: If a UDF is unavoidable, a vectorised pandas UDF is usually much faster than a row-at-a-time Python UDF.
  - id: read-write
    prompt: Which are valid ways to read a Unity Catalog table into a DataFrame?
    type: multiple
    options:
      - text: "`spark.read.table(\"main.sales.orders\")`"
        correct: true
      - text: "`spark.table(\"main.sales.orders\")`"
        correct: true
      - text: "`spark.sql(\"SELECT * FROM main.sales.orders\")`"
        correct: true
      - text: "`open(\"main.sales.orders\").read()`"
    explanation: All three Spark methods return a DataFrame. Python's `open` reads local files, not tables.
---
## Learning objectives

- Transform data with the DataFrame API
- Write equivalent logic in Spark SQL
- Avoid slow Python UDFs

## Reading and writing

```python
orders = spark.read.table("main.sales.orders")

raw = (spark.read
       .format("json")
       .option("multiLine", "true")
       .load("/Volumes/main/raw/landing/orders/"))

(orders.write
       .mode("overwrite")
       .saveAsTable("main.sales.orders_snapshot"))
```

On Databricks, `saveAsTable` creates a Delta table by default.

## Transformations

```python
from pyspark.sql import functions as F
from pyspark.sql.window import Window

daily = (orders
    .where(F.col("status") == "COMPLETED")
    .withColumn("order_date", F.to_date("ordered_at"))
    .groupBy("order_date", "country")
    .agg(F.sum("total").alias("revenue"), F.countDistinct("customer_id").alias("customers")))

w = Window.partitionBy("country").orderBy("order_date")
with_growth = daily.withColumn("prev_revenue", F.lag("revenue").over(w))
```

## The same in SQL

```sql
WITH daily AS (
  SELECT to_date(ordered_at) AS order_date, country,
         SUM(total) AS revenue, COUNT(DISTINCT customer_id) AS customers
  FROM main.sales.orders
  WHERE status = 'COMPLETED'
  GROUP BY ALL
)
SELECT *, LAG(revenue) OVER (PARTITION BY country ORDER BY order_date) AS prev_revenue
FROM daily;
```

`GROUP BY ALL` groups by every non-aggregated column in the select list.

## Mixing the two

```python
daily.createOrReplaceTempView("daily")
top = spark.sql("SELECT * FROM daily WHERE revenue > :min", args={"min": 10000})
```

Named parameter markers (`:min` with `args=`) avoid SQL injection and string formatting bugs.

## UDFs

```python
import pandas as pd
from pyspark.sql.functions import pandas_udf

@pandas_udf("double")
def to_celsius(f: pd.Series) -> pd.Series:
    return (f - 32) * 5 / 9

readings.withColumn("temp_c", to_celsius("temp_f"))
```

Better yet, here a built-in expression — `(F.col("temp_f") - 32) * 5 / 9` — does the job with no
UDF at all.
