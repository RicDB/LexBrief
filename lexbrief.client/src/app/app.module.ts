import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { PreviewDocumentComponent } from './preview-document/preview-document.component';
import { CommentsListComponent } from './comments-list/comments-list.component';
import { DocumentsListComponent } from './documents-list/documents-list.component';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SentimentOverviewComponent } from './sentiment-overview/sentiment-overview.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    CommonModule,
    RouterOutlet,
    DocumentsListComponent,
    CommentsListComponent,
    PreviewDocumentComponent,
    MaterialModule,
    BrowserAnimationsModule,
    SentimentOverviewComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
