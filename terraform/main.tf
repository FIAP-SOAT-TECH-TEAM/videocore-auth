module "info" {
  source = "./modules/info"

  videocore-backend-container        = var.videocore-backend-container
  videocore-backend-infra-key        = var.videocore-backend-infra-key
  videocore-backend-resource-group   = var.videocore-backend-resource-group
  videocore-backend-storage-account  = var.videocore-backend-storage-account
  subscription_id                   = var.subscription_id

}