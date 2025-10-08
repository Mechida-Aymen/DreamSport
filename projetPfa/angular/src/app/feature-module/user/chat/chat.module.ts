import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChatRoutingModule } from './chat-routing.module';
import { ChatComponent } from './chat.component';
import { sharedModule } from 'src/app/shared/shared.module';
import { TruncatePipe } from 'src/app/shared/shared.index';


@NgModule({
  declarations: [
    ChatComponent,
    TruncatePipe
    
  ],
  imports: [
    CommonModule,
    ChatRoutingModule,
    sharedModule
  ]
})
export class ChatModule { }
