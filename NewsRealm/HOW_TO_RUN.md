# Инструкция по запуску проекта NewsRealm

## Требования

1. **.NET 8.0 SDK** - должен быть установлен на вашем компьютере
   - Проверить: `dotnet --version` (должно быть 8.0.x или выше)
   - Скачать: https://dotnet.microsoft.com/download/dotnet/8.0

2. **PostgreSQL** - база данных должна быть запущена
   - Проверить: `psql --version`
   - Скачать: https://www.postgresql.org/download/

## Шаги для запуска

### 1. Проверка установки .NET SDK

Откройте терминал (PowerShell, CMD или Terminal) и выполните:

```bash
dotnet --version
```

Должно отобразиться `8.0.x` или выше. Если нет - установите .NET 8.0 SDK.

### 2. Настройка базы данных PostgreSQL

#### Вариант A: Использование существующей базы данных

Если у вас уже настроена база данных, обновите строку подключения в файле `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "NewsRealmContext": "Host=localhost;Port=5432;Username=ваш_пользователь;Password=ваш_пароль;Database=NewsDB;"
  }
}
```

#### Вариант B: Создание новой базы данных

1. Запустите PostgreSQL
2. Создайте базу данных и пользователя (используйте скрипты из папки `parser/parser/the_news/`):
   - `create_user_and_database.sql`
   - `create_database.sql`

Или выполните вручную:

```sql
CREATE DATABASE NewsDB;
CREATE USER user WITH PASSWORD 'secret';
GRANT ALL PRIVILEGES ON DATABASE NewsDB TO user;
```

3. Убедитесь, что строка подключения в `appsettings.json` соответствует вашим настройкам

### 3. Применение миграций базы данных

Если у вас есть миграции Entity Framework, выполните:

```bash
cd NewsRealm
dotnet ef database update
```

Если миграций нет, создайте их:

```bash
cd NewsRealm
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Восстановление зависимостей

```bash
cd NewsRealm
dotnet restore
```

### 5. Запуск проекта

#### Способ 1: Через Visual Studio / Rider / VS Code
- Просто нажмите F5 или кнопку "Run"
- Проект запустится на `https://localhost:7016` или `http://localhost:5003`

#### Способ 2: Через командную строку

```bash
cd NewsRealm
dotnet run
```

Или с указанием профиля:

```bash
# HTTP только
dotnet run --launch-profile http

# HTTPS (по умолчанию)
dotnet run --launch-profile https
```

### 6. Открытие в браузере

После успешного запуска откройте в браузере:

- **Главная страница фронтенда:** http://localhost:5003/Frontend/Index
- **Добавить новость:** http://localhost:5003/Frontend/AddNews
- **Редактировать новость:** http://localhost:5003/Frontend/EditNews

Или если используете HTTPS:
- https://localhost:7016/Frontend/Index

## Возможные проблемы и решения

### Ошибка подключения к базе данных

```
Npgsql.NpgsqlException: Connection refused
```

**Решение:**
1. Убедитесь, что PostgreSQL запущен
2. Проверьте строку подключения в `appsettings.json`
3. Проверьте, что порт 5432 доступен
4. Убедитесь, что пользователь и пароль правильные

### Ошибка: "Connection string 'NewsRealmContext' not found"

**Решение:**
Проверьте, что в `appsettings.json` есть секция `ConnectionStrings` с ключом `NewsRealmContext`

### Ошибка миграций

Если получаете ошибку о том, что контекст не найден:

```bash
dotnet ef migrations add InitialCreate --project NewsRealm --startup-project NewsRealm
dotnet ef database update --project NewsRealm --startup-project NewsRealm
```

### Порт уже занят

Если порт 5003 или 7016 уже занят, измените порты в `Properties/launchSettings.json`:

```json
"applicationUrl": "https://localhost:7017;http://localhost:5004"
```

## Проверка работоспособности

После запуска:

1. Откройте http://localhost:5003/Frontend/Index
2. Вы должны увидеть главную страницу с лентой новостей
3. Попробуйте добавить новую новость через кнопку "Добавить новость"
4. Проверьте редактирование через кнопку "Редактировать"

## Структура URL

- `/Frontend/Index` - главная страница
- `/Frontend/AddNews` - добавить новость
- `/Frontend/EditNews` - редактировать новость (без ID покажет форму поиска)
- `/Frontend/EditNews?id=1` - редактировать новость с ID=1
- `/NewsModels/Index` - админ-панель для управления новостями

## Дополнительная информация

- Проект использует Entity Framework Core для работы с базой данных
- Все статические файлы (CSS, JS) находятся в папке `wwwroot/`
- Views находятся в папке `Views/Frontend/`
- Контроллеры находятся в папке `Controllers/`

