---
title: Multiple result sets and stored procedures
questions:
  - id: query-multiple
    prompt: What does `QueryMultipleAsync` return?
    options:
      - text: A list of lists
      - text: A `DataSet`
      - id: grid-reader
        text: A `GridReader` from which you read each result set in order with `Read`/`ReadAsync` methods
        correct: true
      - text: The number of result sets
    explanation: Result sets must be read in the order the SQL produces them, and the reader must be disposed.
  - id: proc
    prompt: How do you call a stored procedure with Dapper?
    options:
      - text: Prefix the name with `EXEC` and nothing else is possible
      - id: commandtype
        text: "Pass the procedure name as the SQL and set `commandType: CommandType.StoredProcedure`"
        correct: true
      - text: Dapper does not support stored procedures
    explanation: With `CommandType.StoredProcedure`, the parameter object's properties become the procedure's parameters.
  - id: benefits
    prompt: Which are benefits of `QueryMultiple` over separate queries?
    type: multiple
    options:
      - text: One round trip to the database for several result sets
        correct: true
      - text: Avoids duplicating parent data the way a large JOIN would
        correct: true
      - text: Result sets can be read in any order
      - text: The batch can share variables and temp tables within one command
        correct: true
    explanation: Fewer round trips and no row multiplication; the price is reading results strictly in order.
---
## Learning objectives

- Read several result sets in one round trip
- Call stored procedures with input and output parameters

## QueryMultiple

```csharp
const string sql = """
    SELECT Id, CustomerId, PlacedAt, Total FROM Orders WHERE Id = @id;
    SELECT Id, Sku, Quantity, UnitPrice FROM OrderLines WHERE OrderId = @id;
    SELECT Id, Status, ChangedAt FROM OrderHistory WHERE OrderId = @id ORDER BY ChangedAt;
    """;

using var multi = await db.QueryMultipleAsync(sql, new { id });

var order = await multi.ReadSingleOrDefaultAsync<OrderDetails>();
if (order is null) return null;

order.Lines = (await multi.ReadAsync<OrderLine>()).ToList();
order.History = (await multi.ReadAsync<OrderStatusChange>()).ToList();
return order;
```

## Stored procedures

```csharp
var top = await db.QueryAsync<BestSeller>(
    "dbo.GetBestSellers",
    new { Category = "Books", Top = 10 },
    commandType: CommandType.StoredProcedure);
```

With output and return values:

```csharp
var p = new DynamicParameters(new { CustomerId = customerId, Amount = amount });
p.Add("@NewBalance", dbType: DbType.Decimal, direction: ParameterDirection.Output, precision: 18, scale: 2);
p.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

await db.ExecuteAsync("dbo.ApplyCredit", p, commandType: CommandType.StoredProcedure);

var balance = p.Get<decimal>("@NewBalance");
var status = p.Get<int>("@Result");
```

Output parameter values are only available after the command has completed (and, for queries,
after the results have been consumed).

## Table-valued parameters (SQL Server)

```csharp
var table = new DataTable();
table.Columns.Add("Id", typeof(int));
foreach (var id in ids) table.Rows.Add(id);

var rows = await db.QueryAsync<Product>(
    "dbo.GetProductsByIds",
    new { Ids = table.AsTableValuedParameter("dbo.IntList") },
    commandType: CommandType.StoredProcedure);
```

TVPs scale far beyond the 2,100-parameter limit of `IN @list` expansion.
