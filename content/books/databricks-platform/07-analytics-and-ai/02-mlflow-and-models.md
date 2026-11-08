---
title: MLflow and models in Unity Catalog
questions:
  - id: tracking
    prompt: What does MLflow Tracking record?
    options:
      - text: Only the final model file
      - text: Cluster CPU usage
      - text: Git commits
      - id: runs
        text: Runs with their parameters, metrics, tags and artefacts, so experiments can be compared and reproduced
        correct: true
    explanation: "`mlflow.autolog()` captures most of this automatically for popular libraries."
  - id: registry
    prompt: How are models registered in Unity Catalog addressed?
    options:
      - id: three-level
        text: With a three-level name such as `prod.ml.churn_model`, governed by Unity Catalog privileges
        correct: true
      - text: With a workspace-local name only
      - text: By the run ID only
    explanation: "Set `mlflow.set_registry_uri(\"databricks-uc\")` and register models under catalog.schema.model."
  - id: aliases
    prompt: Which statements about model aliases are true?
    type: multiple
    options:
      - text: An alias such as `champion` points to a specific model version
        correct: true
      - text: "Inference code can load `models:/prod.ml.churn_model@champion` and pick up new versions when the alias moves"
        correct: true
      - text: Aliases replace the older workspace registry stages such as Staging and Production
        correct: true
      - text: A model version can never have more than one alias
    explanation: Aliases are mutable, named pointers. A version may carry several aliases.
---
## Learning objectives

- Track experiments with MLflow
- Register and govern models in Unity Catalog
- Promote models with aliases

## Tracking

```python
import mlflow
from sklearn.ensemble import RandomForestClassifier
from sklearn.metrics import roc_auc_score

mlflow.autolog()

with mlflow.start_run(run_name="rf-baseline"):
    model = RandomForestClassifier(n_estimators=200, max_depth=8).fit(X_train, y_train)
    auc = roc_auc_score(y_test, model.predict_proba(X_test)[:, 1])
    mlflow.log_metric("test_auc", auc)
```

The experiment UI compares runs side by side, and each run links to the code version and data
used.

## Registering in Unity Catalog

```python
mlflow.set_registry_uri("databricks-uc")

result = mlflow.register_model(
    model_uri=f"runs:/{run_id}/model",
    name="prod.ml.churn_model")
```

Models in Unity Catalog get the same governance as tables: privileges, lineage back to training
tables, and audit logs.

## Aliases

```python
from mlflow import MlflowClient

client = MlflowClient()
client.set_registered_model_alias("prod.ml.churn_model", "champion", result.version)

champion = mlflow.pyfunc.load_model("models:/prod.ml.churn_model@champion")
```

A typical promotion flow: register a new version, validate it (metrics, bias checks) as
`challenger`, then move `champion` to it. Consumers never change their code.

## Batch inference

```python
predict = mlflow.pyfunc.spark_udf(spark, "models:/prod.ml.churn_model@champion")
scored = spark.read.table("prod.gold.customer_features").withColumn("churn_score", predict(*feature_cols))
scored.write.mode("overwrite").saveAsTable("prod.gold.churn_scores")
```

## Features

Feature tables in Unity Catalog (any Delta table with a primary key) can be joined automatically
at training time and looked up at serving time, preventing training/serving skew.
