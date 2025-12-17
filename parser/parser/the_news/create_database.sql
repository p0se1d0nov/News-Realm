-- SQL скрипт для создания таблиц и индексов в базе данных PostgreSQL
-- Основан на структуре данных из output.json
-- 
-- ВНИМАНИЕ: Этот скрипт выполняется от имени пользователя news_database_admin
-- 
-- Перед выполнением этого скрипта убедитесь, что:
-- 1. Пользователь news_database_admin создан (используйте create_user_and_database.sql)
-- 2. База данных the_news создана
-- 
-- Использование:
-- psql -U news_database_admin -d the_news -f create_database.sql

-- Создание таблицы новостей
CREATE TABLE IF NOT EXISTS news (
    id SERIAL PRIMARY KEY,
    source VARCHAR(50) NOT NULL DEFAULT 'rss',
    title TEXT NOT NULL,
    description TEXT,
    maintext TEXT,
    image_url TEXT,
    authors VARCHAR(500),
    category VARCHAR(100),
    date_publish DATE,
    time_publish TIME,
    language VARCHAR(10) NOT NULL DEFAULT 'ru',
    url TEXT NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Создание индексов для оптимизации запросов
CREATE INDEX IF NOT EXISTS idx_news_category ON news(category);
CREATE INDEX IF NOT EXISTS idx_news_date_publish ON news(date_publish);
CREATE INDEX IF NOT EXISTS idx_news_language ON news(language);
CREATE INDEX IF NOT EXISTS idx_news_source ON news(source);
CREATE INDEX IF NOT EXISTS idx_news_created_at ON news(created_at);
CREATE INDEX IF NOT EXISTS idx_news_url ON news(url);

-- Предоставление всех привилегий на таблицу news пользователю news_database_admin
-- (это необходимо, так как таблица создается в схеме public)
GRANT ALL PRIVILEGES ON TABLE news TO news_database_admin;
GRANT USAGE, SELECT ON SEQUENCE news_id_seq TO news_database_admin;

-- Отзыв прав доступа у других пользователей (опционально, для безопасности)
-- REVOKE ALL ON TABLE news FROM PUBLIC;
-- REVOKE ALL ON SEQUENCE news_id_seq FROM PUBLIC;

-- Комментарии к таблице и полям
COMMENT ON TABLE news IS 'Таблица для хранения новостей, собранных парсером';
COMMENT ON COLUMN news.id IS 'Уникальный идентификатор записи';
COMMENT ON COLUMN news.source IS 'Источник новости (например, rss)';
COMMENT ON COLUMN news.description IS 'Краткое описание новости';
COMMENT ON COLUMN news.maintext IS 'Полный текст новости';
COMMENT ON COLUMN news.image_url IS 'URL изображения к новости';
COMMENT ON COLUMN news.authors IS 'Автор(ы) новости';
COMMENT ON COLUMN news.category IS 'Категория новости (politics, sport, society и т.д.)';
COMMENT ON COLUMN news.date_publish IS 'Дата публикации новости';
COMMENT ON COLUMN news.time_publish IS 'Время публикации новости';
COMMENT ON COLUMN news.language IS 'Язык новости (ru, en и т.д.)';
COMMENT ON COLUMN news.url IS 'URL оригинальной новости (уникальный)';
COMMENT ON COLUMN news.created_at IS 'Время добавления записи в базу данных';

-- ============================================
-- ОБЪЯСНЕНИЕ ИНДЕКСОВ
-- ============================================
-- 
-- Индексы - это специальные структуры данных, которые ускоряют поиск и сортировку в базе данных.
-- 
-- Представьте библиотеку без каталога: чтобы найти книгу, вам нужно просмотреть все полки.
-- Индекс - это как каталог библиотеки: он указывает, где именно находится нужная информация.
-- 
-- ПРЕИМУЩЕСТВА ИНДЕКСОВ:
-- 1. Ускорение поиска: запросы с WHERE по индексированным полям выполняются намного быстрее
-- 2. Ускорение сортировки: ORDER BY по индексированным полям работает быстрее
-- 3. Ускорение JOIN операций: соединение таблиц по индексированным полям эффективнее
-- 
-- НЕДОСТАТКИ ИНДЕКСОВ:
-- 1. Занимают дополнительное место на диске
-- 2. Замедляют операции INSERT/UPDATE/DELETE (так как нужно обновлять индекс)
-- 
-- В НАШЕМ СЛУЧАЕ:
-- - idx_news_category: ускоряет поиск новостей по категориям (SELECT * FROM news WHERE category = 'politics')
-- - idx_news_date_publish: ускоряет поиск по датам и сортировку по дате публикации
-- - idx_news_language: ускоряет фильтрацию по языку
-- - idx_news_source: ускоряет поиск по источнику новостей
-- - idx_news_created_at: ускоряет сортировку по времени добавления (ORDER BY created_at DESC)
-- - idx_news_url: ускоряет проверку уникальности URL и поиск по URL
-- 
-- БЕЗ ИНДЕКСОВ: PostgreSQL пришлось бы просматривать ВСЕ строки таблицы (полное сканирование таблицы)
-- С ИНДЕКСАМИ: PostgreSQL использует индекс для быстрого поиска нужных строк
