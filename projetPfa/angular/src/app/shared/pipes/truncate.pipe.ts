// src/app/shared/pipes/truncate.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate'
})
export class TruncatePipe implements PipeTransform {
  transform(value: string | null | undefined, limit: number = 20): string {
    // Gestion des valeurs null/undefined
    if (!value) return '';
    
    // Conversion en string pour les nombres et autres types
    const stringValue = String(value);
    
    return stringValue.length > limit ? 
           stringValue.substring(0, limit) + '...' : 
           stringValue;
  }
}