-- Скрипт для создания пользователя и базы данных
-- ВЫПОЛНЯЕТСЯ ОТ ИМЕНИ СУПЕРПОЛЬЗОВАТЕЛЯ PostgreSQL (postgres)
-- 
-- Использование:
-- sudo -u postgres psql -f create_user_and_database.sql
-- или
-- sudo -u postgres psql
-- \i create_user_and_database.sql

-- Создание пользователя news_database_admin
-- ВАЖНО: Измените пароль на более безопасный перед выполнением!
CREATE USER news_database_admin WITH PASSWORD '1234567890';

-- Создание базы данных с владельцем news_database_admin
CREATE DATABASE the_news
    WITH OWNER = news_database_admin
    ENCODING = 'UTF8'
    LC_COLLATE = 'ru_RU.UTF-8'
    LC_CTYPE = 'ru_RU.UTF-8'
    TEMPLATE = template0;

-- Предоставление всех привилегий пользователю на базу данных
GRANT ALL PRIVILEGES ON DATABASE the_news TO news_database_admin;

-- Вывод сообщения об успешном создании
\echo 'Пользователь news_database_admin и база данных the_news успешно созданы!'
\echo 'Теперь выполните: psql -U news_database_admin -d the_news -f create_database.sql'
