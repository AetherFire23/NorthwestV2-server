docker run -d `
  --name northwest-postgres `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=123 `
  -e POSTGRES_DB=northwest `
  -p 5432:5432 `
  postgres