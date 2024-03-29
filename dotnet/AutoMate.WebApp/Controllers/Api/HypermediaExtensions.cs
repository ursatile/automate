﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using AutoMate.Data.Entities;
using Newtonsoft.Json;

namespace AutoMate.WebApp.Controllers.Api {
    public static class HypermediaExtensions {

        public static dynamic ToResource(this Vehicle vehicle) {
            var resource = vehicle.ToDynamic();
            resource._links = new {
                self = new { href = $"/api/vehicles/{vehicle.Registration}"  },
                model = new { href = $"/api/vehiclemodels/{vehicle.ModelCode}" },
            };
            return resource;
        }

        public static dynamic ToResource(this VehicleModel vehicleModel) {
            var resource = vehicleModel.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/vehiclemodels/{vehicleModel.Code}"
                }
            };
            return resource;
        }

        public static dynamic ToDynamic(this object value) {
            IDictionary<string, object> expando = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor property in properties) {
                if (Ignore(property)) continue;
                expando.Add(property.Name, property.GetValue(value));
            }
            return (ExpandoObject)expando;
        }

        private static bool Ignore(PropertyDescriptor prop) {
            return prop.Attributes.OfType<JsonIgnoreAttribute>().Any();
        }
    }
}