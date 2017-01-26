﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoralesLarios.CustomsControls.Extensions;
using System.Windows.Data;
using System.ComponentModel;
using MoralesLarios.CustomsControls.Helpers;

namespace MoralesLarios.CustomsControls.HelpControls
{
    public class SupportFilteresSearch : ISupportFilteresSearch
    {

        private readonly IPopulateStackPanelControls populateStackPanelControls;

        private IEnumerable<object> source;

        private ICollectionView view;


        public SupportFilteresSearch(IPopulateStackPanelControls populateStackPanelControls, IEnumerable<object> source)
        {
            this.populateStackPanelControls = populateStackPanelControls;

            this.source = source;

            view = CollectionViewSource.GetDefaultView(this.source);
        }

        public async void FilterItemsSource(object objFilter, Func<object, object, bool> filter, IEnumerable<string> fieldsSearchNames)
        {
            view = CollectionViewSource.GetDefaultView(source);

            await source.FuncFilterAllInFieldsAsync(view, objFilter, filter, fieldsSearchNames);
        }

        public Task FilterItemsSourceAsync(object objFilter, Func<object, object, bool> filter, IEnumerable<string> fieldsSearchNames)
        {
            view = CollectionViewSource.GetDefaultView(source);

            return source.FuncFilterAllInFieldsAsync(view, objFilter, filter, fieldsSearchNames);
        }

        public IEnumerable<object> GetFilteredAllItemSource(IEnumerable<object> itemsSource)
        {
            var view = CollectionViewSource.GetDefaultView(itemsSource);

            IEnumerable<object> result = view.OfType<object>().Take(itemsSource.Count());

            return result;
        }

        public IEnumerable<object> GetFilteredItemSource(IEnumerable<object> itemsSource)
        {
            IEnumerable<object> result = view.OfType<object>();

            return result;
        }





        public void PopulateSubResults(object objFilter, Func<object, object, bool> filter, int count, IEnumerable<string> strFieldsSearchNames)
        {
            /// Posiblemente haya que hacer esto en asíncrono

            populateStackPanelControls.RemoveControls();

            if (!string.IsNullOrEmpty(objFilter?.ToString()))
            {
                var filterResults = source.WhereAllForFuncInOrder(objFilter, filter, count, strFieldsSearchNames)
                //.Select(displaySugerencyMemberPath)
                .ToList();

                populateStackPanelControls.PopulateWithLinks(filterResults);
            }

            
        }

    }
}
