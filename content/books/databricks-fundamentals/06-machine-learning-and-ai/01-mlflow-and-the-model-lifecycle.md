---
title: MLflow and the model lifecycle
questions:
  - id: mlflow
    prompt: What is MLflow?
    options:
      - text: A proprietary Databricks-only tool
      - text: A deep learning framework like PyTorch
      - text: A data visualisation library
      - id: oss
        text: An open-source platform (Linux Foundation project, created by Databricks) for tracking experiments, packaging models, managing a registry, and tracing and evaluating GenAI applications
        correct: true
    explanation: Databricks hosts a managed MLflow with Unity Catalog integration. The open-source version runs anywhere.
  - id: tracking
    prompt: What does MLflow tracking record for a run?
    type: multiple
    options:
      - id: params
        text: Parameters
        correct: true
      - id: metrics
        text: Metrics
        correct: true
      - id: artifacts
        text: Artifacts such as models, plots and data samples
        correct: true
      - text: The developer's salary
    explanation: Autologging captures these for popular libraries with one call, `mlflow.autolog()`.
  - id: registry-uc
    prompt: Where are registered models stored on Databricks today?
    options:
      - id: uc
        text: In Unity Catalog, as `catalog.schema.model`, with versions, aliases (such as `champion`) and lineage to training data
        correct: true
      - text: In the legacy workspace model registry only
      - text: In Git
      - text: In the driver's memory
    explanation: Aliases replaced the older "stages" (Staging/Production). Deployment code refers to `models:/main.ml.churn@champion`.
  - id: alias
    prompt: Why use an alias like `@champion` in serving code rather than a version number?
    options:
      - text: Aliases make models faster
      - id: promote
        text: Promoting a new version means moving the alias; serving code does not change
        correct: true
      - text: Version numbers are not supported
      - text: Aliases are required by Spark
    explanation: Combine with an approval process so alias moves are deliberate and auditable.
  - id: lifecycle
    prompt: Which order best describes a typical ML lifecycle?
    options:
      - text: Deploy → collect data → train → evaluate
      - text: Train → deploy → evaluate → collect data
      - id: order
        text: Prepare data and features → train and track experiments → evaluate → register → deploy → monitor → retrain
        correct: true
      - text: Evaluate → train → collect data → deploy
    explanation: Monitoring feeds back into retraining. Skipping monitoring is the most common gap in practice.
  - id: drift
    prompt: What is data drift?
    options:
      - text: Files moving between storage accounts
      - text: Clock skew between servers
      - text: A slow network
      - id: drift
        text: A change in the statistical distribution of model inputs (or of the relationship between inputs and the target) over time, which degrades model quality
        correct: true
    explanation: Data quality monitoring (formerly Lakehouse Monitoring) can profile tables and inference logs to detect drift.
  - id: feature-store
    prompt: What does a feature store provide?
    options:
      - id: features
        text: Reusable, governed feature tables with lineage, plus consistent feature lookup at training and serving time to avoid training/serving skew
        correct: true
      - text: A shop selling ML models
      - text: A cache for dashboards
      - text: A Git repository
    explanation: In Unity Catalog any Delta table with a primary key can act as a feature table; online stores serve low-latency lookups.
  - id: automl
    prompt: Which statement about AutoML is fair?
    options:
      - text: It replaces data scientists entirely
      - text: It produces only black-box models
      - text: It works without any data
      - id: baseline
        text: It quickly produces baseline models and editable notebooks; it does not replace problem framing, feature engineering or validation
        correct: true
    explanation: AutoML is good for a starting point. Many failures in ML come from framing and data leakage, which AutoML cannot detect for you.
  - id: leakage
    prompt: A churn model scores 99% accuracy in testing but fails in production. Which is a likely cause?
    options:
      - text: The cluster was too small
      - id: leakage
        text: Data leakage — a feature that encodes the outcome (for example "cancellation_date") was available in training but not at prediction time
        correct: true
      - text: MLflow was not used
      - text: The model was registered in Unity Catalog
    explanation: Too-good-to-be-true results usually are. Also check class imbalance — 99% accuracy is trivial if 99% of customers do not churn.
  - id: open-portable
    prompt: Why does it matter that MLflow's model format is open?
    options:
      - text: It makes training faster
      - text: It is required by law
      - id: portable
        text: Models logged with MLflow can be loaded and served outside Databricks, reducing lock-in for the ML layer
        correct: true
      - text: It encrypts models
    explanation: Managed features (Unity Catalog lineage, serving, monitoring) remain Databricks-specific.
---
## Learning objectives

- Describe MLflow's components
- Register, alias and promote models in Unity Catalog
- Recognise drift, leakage and other lifecycle failure modes

## Tracking and registering

```python
import mlflow
from sklearn.ensemble import RandomForestClassifier

mlflow.set_registry_uri("databricks-uc")
mlflow.autolog()

with mlflow.start_run():
    model = RandomForestClassifier(n_estimators=200).fit(X_train, y_train)
    mlflow.sklearn.log_model(model, name="model",
                             registered_model_name="main.ml.churn",
                             input_example=X_train.head(5))
```

```python
from mlflow import MlflowClient
MlflowClient().set_registered_model_alias("main.ml.churn", "champion", version=7)
model = mlflow.pyfunc.load_model("models:/main.ml.churn@champion")
```

## Sources

- MLflow documentation — <https://mlflow.org/docs/latest/>
- Databricks docs, "Manage model lifecycle in Unity Catalog" — <https://docs.databricks.com/aws/en/machine-learning/manage-model-lifecycle/>
- Databricks docs, "Feature engineering in Unity Catalog" — <https://docs.databricks.com/aws/en/machine-learning/feature-store/>
- Kaufman et al., "Leakage in Data Mining", ACM TKDD 2012 — <https://dl.acm.org/doi/10.1145/2382577.2382579>
