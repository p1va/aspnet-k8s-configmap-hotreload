# Hot-reload configs in a ASP.NET app running in Kubernetes

## Intro

This is a demo project showcasing how to configure a ASP.NET Core app and its Kubernetes deployment so that changes applied to `ConfigMap`s take effect without requiring an app restart.

## Build and deploy

This demo was intended to run in a `microk8s` local cluster which exposed its container registry at `localhost:32000`.

### Build image

```sh
docker-compose build
```
### Push image to container registry

```sh
docker-compose push
```
This can be double checked by querying container registry APIs.

```sh
curl http://localhost:32000/v2/hotreload/tags/list | jq .
```

### Create demo namespace

```sh
k apply -f deploy/namespace.yaml
```
then switch to the newly crated `hotreload-demo` namespace

### Apply resources

```sh
k apply -f deploy/providers-configmap.yaml
k apply -f deploy/featuremanagement-configmap.yaml
k apply -f deploy/deployment.yaml
k apply -f deploy/service.yaml
```

### Call APIs locally

Port forward to the app service on port `5078`

```sh
k port-forward svc/hotrealod 5078:5078
```
Then call `/providers` endpoint

```sh
curl -s http://localhost:5078/providers | jq .
```

And `/authuri`

```sh
curl -s http://localhost:5078/authuri | jq .
```
### Change configuration without restart

You can change configuration by updating the content of `deploy/providers-configmap.yaml` and `deploy/featuremanagement-configmap.yaml`.

To make the change effective apply it to the cluster

```sh
k apply -f deploy/providers-configmap.yaml
k apply -f deploy/featuremanagement-configmap.yaml
```
Now wait 1 minute for the changes to be synced and then verify that by invoking APIs again

```sh
curl -s http://localhost:5078/providers | jq .
curl -s http://localhost:5078/authuri | jq .
```