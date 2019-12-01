# Chili Web App

## Database-First
```
cd Usa.chili.Web
dotnet ef dbcontext scaffold "server=localhost;port=3306;user=root;password=root;database=chili" MySql.Data.EntityFrameworkCore -f --project ../Usa.chili.Domain
rm -f ../Usa.chili.Domain/chiliContext.cs
```

## Bundling/Minimization
```
cd Usa.chili.Web
dotnet bundle
```

# Service location
`/etc/systemd/system/kestrel-chili.service`