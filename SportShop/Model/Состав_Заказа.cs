//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportShop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Состав_Заказа
    {
        public int id_Состав_Заказ { get; set; }
        public Nullable<int> Заказ { get; set; }
        public Nullable<int> Товар { get; set; }
        public Nullable<int> Колличество { get; set; }
    
        public virtual Заказ Заказ1 { get; set; }
        public virtual Товар Товар1 { get; set; }
    }
}
