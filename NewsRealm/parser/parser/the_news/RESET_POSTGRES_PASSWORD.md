# Как сбросить пароль пользователя postgres в PostgreSQL

## Способ 1: Через sudo (самый простой)

Если у вас есть права sudo, вы можете подключиться к PostgreSQL без пароля:

```bash
# Подключитесь к PostgreSQL от имени системного пользователя postgres
sudo -u postgres psql

# В psql выполните команду для сброса пароля
ALTER USER postgres WITH PASSWORD 'новый_пароль';

# Выйдите из psql
\q
```

## Способ 2: Изменение файла pg_hba.conf (если способ 1 не работает)

Если способ 1 не работает, временно измените настройки аутентификации:

### Шаг 1: Найдите файл pg_hba.conf

```bash
# Для PostgreSQL установленного через пакетный менеджер
sudo find /etc/postgresql -name pg_hba.conf

# Или проверьте стандартные расположения
ls /etc/postgresql/*/main/pg_hba.conf
# или
ls /var/lib/pgsql/data/pg_hba.conf
```

### Шаг 2: Сделайте резервную копию

```bash
sudo cp /etc/postgresql/*/main/pg_hba.conf /etc/postgresql/*/main/pg_hba.conf.backup
```

### Шаг 3: Временно измените метод аутентификации

Откройте файл `pg_hba.conf`:

```bash
sudo nano /etc/postgresql/*/main/pg_hba.conf
```

Найдите строку, которая выглядит примерно так:
```
local   all             postgres                                peer
# или
host    all             postgres        127.0.0.1/32            md5
```

Измените метод аутентификации на `trust` (только для локальных подключений):
```
local   all             postgres                                trust
host    all             postgres        127.0.0.1/32            trust
```

**ВАЖНО:** Метод `trust` позволяет подключаться без пароля! Используйте его только временно!

### Шаг 4: Перезапустите PostgreSQL

```bash
# Для systemd (большинство современных Linux дистрибутивов)
sudo systemctl restart postgresql

# Или для старых систем
sudo service postgresql restart
```

### Шаг 5: Подключитесь и сбросьте пароль

```bash
# Теперь можно подключиться без пароля
psql -U postgres

# Сбросьте пароль
ALTER USER postgres WITH PASSWORD 'новый_пароль';

# Выйдите
\q
```

### Шаг 6: Верните настройки безопасности

Верните файл `pg_hba.conf` к исходному состоянию:

```bash
# Восстановите из резервной копии
sudo cp /etc/postgresql/*/main/pg_hba.conf.backup /etc/postgresql/*/main/pg_hba.conf

# Или вручную измените обратно на peer/md5
sudo nano /etc/postgresql/*/main/pg_hba.conf
```

Измените обратно:
```
local   all             postgres                                peer
host    all             postgres        127.0.0.1/32            md5
```

### Шаг 7: Снова перезапустите PostgreSQL

```bash
sudo systemctl restart postgresql
```

## Способ 3: Через однопользовательский режим PostgreSQL

Если предыдущие способы не работают:

### Шаг 1: Остановите PostgreSQL

```bash
sudo systemctl stop postgresql
```

### Шаг 2: Запустите PostgreSQL в однопользовательском режиме

```bash
sudo -u postgres /usr/lib/postgresql/*/bin/postgres --single -D /var/lib/postgresql/*/main postgres
```

(Замените `*` на версию вашего PostgreSQL, например `14` или `15`)

### Шаг 3: В консоли PostgreSQL выполните:

```sql
ALTER USER postgres WITH PASSWORD 'новый_пароль';
```

### Шаг 4: Выйдите и перезапустите PostgreSQL

Нажмите `Ctrl+D` для выхода, затем:

```bash
sudo systemctl start postgresql
```

## Проверка

После сброса пароля проверьте подключение:

```bash
psql -U postgres -h localhost
```

Введите новый пароль при запросе.

## Безопасность

После сброса пароля:

1. ✅ Используйте надежный пароль (минимум 12 символов, буквы, цифры, символы)
2. ✅ Убедитесь, что `pg_hba.conf` использует `md5` или `scram-sha-256`, а не `trust`
3. ✅ Ограничьте доступ к базе данных только необходимым пользователям
4. ✅ Регулярно делайте резервные копии

## Для вашего проекта

Если вы забыли пароль для `news_database_admin`, вы можете сбросить его от имени `postgres`:

```bash
sudo -u postgres psql

# Сбросьте пароль для news_database_admin
ALTER USER news_database_admin WITH PASSWORD 'новый_пароль';

\q
```

Затем обновите пароль в файле `settings.py` вашего проекта.
