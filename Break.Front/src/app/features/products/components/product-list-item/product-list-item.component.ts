import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { CurrencyPipe, NgClass, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Item } from '../../../../core/models/item.model'; // Adjust the import path as necessary

@Component({
  selector: 'app-product-list-item',
  standalone: true,
  imports: [
    RouterLink, // For navigation links
    CurrencyPipe, // To format currency
    NgClass, // For conditional styling (e.g., low stock)
  ],
  templateUrl: './product-list-item.component.html',
  styleUrls: ['./product-list-item.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush // OnPush for performance
})
export class ProductListItemComponent {
  // Receive product data from the parent component
  @Input({ required: true }) item!: Item;

  // Emit an event when the delete button is clicked
  @Output() deleteRequest = new EventEmitter<Item>();
  // @Output() editRequest = new EventEmitter<Item>(); // Optional: if edit is triggered here

  // FR-INV-03: Determine if stock is low
  get isLowStock(): boolean {
    return this.item.quantityInStock <= this.item.minimumStockLevel;
  }

  onDeleteClick(): void {
    // Emit the item to be deleted to the parent component
    this.deleteRequest.emit(this.item);
  }

  // onEditClick(): void {
  //   this.editRequest.emit(this.item); // Or navigate directly using RouterLink
  // }
}