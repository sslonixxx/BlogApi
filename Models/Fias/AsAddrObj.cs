﻿using System;
using System.Collections.Generic;

namespace blog_api.Models.Fias;

/// <summary>
/// Сведения классификатора адресообразующих элементов
/// </summary>
public partial class AsAddrObj
{
    /// <summary>
    /// Уникальный идентификатор записи. Ключевое поле
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Глобальный уникальный идентификатор адресного объекта типа INTEGER
    /// </summary>
    public long Objectid { get; set; }

    /// <summary>
    /// Глобальный уникальный идентификатор адресного объекта типа UUID
    /// </summary>
    public Guid Objectguid { get; set; }

    /// <summary>
    /// ID изменившей транзакции
    /// </summary>
    public long? Changeid { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Краткое наименование типа объекта
    /// </summary>
    public string? Typename { get; set; }

    /// <summary>
    /// Уровень адресного объекта
    /// </summary>
    public string Level { get; set; } = null!;

    /// <summary>
    /// Статус действия над записью – причина появления записи
    /// </summary>
    public int? Opertypeid { get; set; }

    /// <summary>
    /// Идентификатор записи связывания с предыдущей исторической записью
    /// </summary>
    public long? Previd { get; set; }

    /// <summary>
    /// Идентификатор записи связывания с последующей исторической записью
    /// </summary>
    public long? Nextid { get; set; }

    /// <summary>
    /// Дата внесения (обновления) записи
    /// </summary>
    public DateOnly? Updatedate { get; set; }

    /// <summary>
    /// Начало действия записи
    /// </summary>
    public DateOnly? Startdate { get; set; }

    /// <summary>
    /// Окончание действия записи
    /// </summary>
    public DateOnly? Enddate { get; set; }

    /// <summary>
    /// Статус актуальности адресного объекта ФИАС
    /// </summary>
    public int? Isactual { get; set; }

    /// <summary>
    /// Признак действующего адресного объекта
    /// </summary>
    public int? Isactive { get; set; }
    
}
