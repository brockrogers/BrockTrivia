//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TriviaService
{
    using System;
    using System.Collections.Generic;
    
    public partial class GamePlayer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GamePlayer()
        {
            this.GameAnswers = new HashSet<GameAnswer>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid GameRoomId { get; set; }
        public string PlayerName { get; set; }
    
        public virtual GameRoom GameRoom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameAnswer> GameAnswers { get; set; }
    }
}
