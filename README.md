# Postman

### 🔗 Workspace: https://www.postman.com/pedroferrarezzo-2950189/workspace/fiap-soat-tech-team-8

### ❓ Como preencher Variáveis:
#### VideoCore Auth Collection 🔑
- **azFuncLocalUrl:** `http://localhost:7025`
- **cognitoDomainUrl:** `https://videocore-auth.auth.sa-east-1.amazoncognito.com`
    > ℹ️ Ou consultar output terraform: `cognito_code_get_token_url`

#### VideoCore Reports Collections 🌐⚙️🔄
> ⚠️ Os endpoints `HTTP` e `WebSocket` estão distribuídos em diferentes `collections` pois o `Postman` não suporta a criação de uma única contendo diferentes tipos de `endpoint`.
- **reportsHttpLocalUrl:** `http://localhost:8081`
- **reportsHttpsApimUrl:** `https://videocore-apim.azure-api.net` 
    > ℹ️ Ou consultar output terraform: `apim_gateway_url`
- **reportsWsLocalUrl:** `ws://localhost:8081`
- **reportsWssApimUrl:** `wss://videocore-apim.azure-api.net`
    > ℹ️ Ou consultar output terraform: `apim_gateway_url`
- **videoCoreStartSubscription:** consultar output terraform: `apim_videocore_start_subscription_key`
    > ℹ️ Ou capturar via `Azure Console`
- **reportsAuthorizationHeader:** consultar `access_token` retornado pelo `Cognito` pós autenticação