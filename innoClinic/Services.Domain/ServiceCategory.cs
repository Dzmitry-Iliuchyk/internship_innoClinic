﻿using Shared.Abstractions.Entities;

namespace Services.Domain {
    public class ServiceCategory : Entity<int> {
        public string Name { get; set; }
        public TimeSpan TimeSlotSize { get; set; }
    }
}
