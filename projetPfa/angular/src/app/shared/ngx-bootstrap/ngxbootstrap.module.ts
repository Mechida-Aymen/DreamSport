import { NgModule } from '@angular/core';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { LightboxModule } from 'ngx-lightbox';

@NgModule({
  imports: [
    PopoverModule.forRoot(),
    TooltipModule.forRoot(),
    LightboxModule,
  ],
  exports: [
    PopoverModule,
    TooltipModule,
    LightboxModule,
  ],
  providers: [
    
  ],
})
export class NgxbootstrapModule {}
