//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DEMKA1231231321.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductsMaterials
    {
        public string product_id { get; set; }
        public int material_id { get; set; }
        public Nullable<int> count { get; set; }
    
        public virtual Materials Materials { get; set; }
        public virtual Products Products { get; set; }
    }
}
