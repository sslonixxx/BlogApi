﻿using System;
using System.Collections.Generic;

namespace blog_api.Models.Fias;

/// <summary>
/// Сведения по номерам домов улиц городов и населенных пунктов
/// </summary>
public partial class AsHouse
{
    /// <summary>
    /// Уникальный идентификатор записи. Ключевое поле
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Глобальный уникальный идентификатор объекта типа INTEGER
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
    /// Основной номер дома
    /// </summary>
    public string? Housenum { get; set; }

    /// <summary>
    /// Дополнительный номер дома 1
    /// </summary>
    public string? Addnum1 { get; set; }

    /// <summary>
    /// Дополнительный номер дома 1
    /// </summary>
    public string? Addnum2 { get; set; }

    /// <summary>
    /// Основной тип дома
    /// </summary>
    public int? Housetype { get; set; }

    /// <summary>
    /// Дополнительный тип дома 1
    /// </summary>
    public int? Addtype1 { get; set; }

    /// <summary>
    /// Дополнительный тип дома 2
    /// </summary>
    public int? Addtype2 { get; set; }

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
