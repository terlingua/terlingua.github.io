---
title: Privileges and fine-grained access
questions:
  - id: minimum
    prompt: A group needs to query `prod.sales.orders`. Which set of privileges is the minimum required?
    options:
      - text: "`SELECT` on the table only"
      - id: three
        text: "`USE CATALOG` on `prod`, `USE SCHEMA` on `prod.sales`, and `SELECT` on the table"
        correct: true
      - text: "`ALL PRIVILEGES` on the catalog"
      - text: Workspace admin rights
    explanation: Access requires the ability to traverse the catalog and schema plus the object privilege.
  - id: inheritance
    prompt: "What happens when you `GRANT SELECT ON SCHEMA prod.sales TO analysts`?"
    options:
      - id: inherit
        text: Analysts can select from all current and future tables in that schema (given `USE CATALOG` and `USE SCHEMA`)
        correct: true
      - text: Only tables existing at grant time are affected
      - text: It is invalid; SELECT can only be granted on tables
    explanation: Privileges granted on a container are inherited by the objects inside it.
  - id: fine-grained
    prompt: Which Unity Catalog features restrict data within a table?
    type: multiple
    options:
      - text: Row filters, implemented as SQL functions applied to the table
        correct: true
      - text: Column masks, implemented as SQL functions applied to a column
        correct: true
      - text: Dynamic views using functions such as `is_account_group_member()`
        correct: true
      - text: Renaming the table every hour
    explanation: Row filters and column masks enforce restrictions for every query, whatever tool issues it.
---
## Learning objectives

- Grant privileges following least privilege
- Understand inheritance and ownership
- Apply row filters and column masks

## Granting

```sql
GRANT USE CATALOG ON CATALOG prod TO `analysts`;
GRANT USE SCHEMA ON SCHEMA prod.sales TO `analysts`;
GRANT SELECT ON SCHEMA prod.sales TO `analysts`;

GRANT MODIFY, SELECT ON TABLE prod.sales.orders TO `etl-service-principal`;
GRANT READ VOLUME ON VOLUME prod.raw.landing TO `data-engineers`;

SHOW GRANTS ON SCHEMA prod.sales;
REVOKE SELECT ON SCHEMA prod.sales FROM `contractors`;
```

## Common privileges

| Privilege | Allows |
|-----------|--------|
| `USE CATALOG` / `USE SCHEMA` | Traversing into the container |
| `BROWSE` | Seeing metadata without reading data |
| `SELECT` | Reading tables and views |
| `MODIFY` | Inserting, updating, deleting |
| `CREATE TABLE`, `CREATE SCHEMA`, ... | Creating objects |
| `READ VOLUME` / `WRITE VOLUME` | File access in volumes |
| `EXECUTE` | Calling functions, using models |
| `MANAGE` | Managing privileges on an object without owning it |

Every object has an **owner** (a user, group or service principal) with full control. Make groups,
not individuals, the owners of production objects.

## Row filters

```sql
CREATE FUNCTION prod.security.region_filter(region STRING)
RETURN is_account_group_member('global-sales') OR region = current_user_region();

ALTER TABLE prod.sales.orders SET ROW FILTER prod.security.region_filter ON (region);
```

(`current_user_region()` stands in for your own lookup, for example a join against a mapping table.)

## Column masks

```sql
CREATE FUNCTION prod.security.mask_email(email STRING)
RETURN CASE WHEN is_account_group_member('pii-readers') THEN email
            ELSE regexp_replace(email, '^[^@]+', '***') END;

ALTER TABLE prod.sales.customers ALTER COLUMN email SET MASK prod.security.mask_email;
```

Filters and masks apply to every query — notebooks, SQL warehouses, BI tools and Delta Sharing
recipients alike. Newer **attribute-based access control** (ABAC) policies let you apply such rules
by tags across many tables at once.
