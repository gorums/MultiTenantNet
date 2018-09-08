# MultiTenantNet
Multi Tenant Dotnet App (SaaS)

## Introduction

This is a multi-tenant application, one application (SaaS) with multiples database, each tenant has his own database with the same schema, 
the use of single-tenant databases gives strong tenant isolation.

If you need different schema in each single-tenant database this project is easy to change to allow that using multiples DbContext pattern.

![Database per Tenant](./AppVersions.PNG)
