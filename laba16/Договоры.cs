//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace laba16
{
    using System;
    using System.Collections.Generic;
    
    public partial class Договоры
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Договоры()
        {
            this.поставлено = new HashSet<поставлено>();
        }
    
        public int НомерДоговора { get; set; }
        public Nullable<System.DateTime> ДатаДоговора { get; set; }
        public int КодПоставщика { get; set; }
        public string Коментарий { get; set; }
    
        public virtual Поставщики Поставщики { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<поставлено> поставлено { get; set; }
    }
}
