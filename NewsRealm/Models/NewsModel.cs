using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NewsRealm.Models
{
    public class NewsModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Источник обязателен для заполнения")]
        [Display(Name = "Источник")]
        public string Source { get; set; } = string.Empty;

        [Required(ErrorMessage = "Заголовок обязателен для заполнения")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание обязательно для заполнения")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Основной текст обязателен для заполнения")]
        [Display(Name = "Основной текст")]
        public string MainText { get; set; } = string.Empty;

        [Url(ErrorMessage = "Введите корректный URL")]
        [Display(Name = "Ссылка на изображение")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Авторы")]
        public string? Authors { get; set; }

        [Display(Name = "Категория")]
        public string? Category { get; set; }

        public string? DatePublish { get; set; }

        public string? TimePublish { get; set; }

        [Required(ErrorMessage = "Язык обязателен для заполнения")]
        [Display(Name = "Язык")]
        public string Language { get; set; } = "Русский";

        public string? Url { get; set; }

        public string? CreatedAt { get; set; }
    }
}
