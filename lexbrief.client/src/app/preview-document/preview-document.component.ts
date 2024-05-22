import { Component, Input, Pipe, PipeTransform } from '@angular/core';
import {CommonModule} from "@angular/common";
import { DocumentDetailDto, DocumentsListService } from '../documents-list/documents-list.service';
import { tap } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-preview-document',
  standalone: true,
  imports: [CommonModule],
  providers: [DocumentsListService],
  templateUrl: './preview-document.component.html',
  styleUrls: ['./preview-document.component.css']
})
export class PreviewDocumentComponent {
  @Input() set docId(value: number){
    if(value === undefined)
      return;

    this.getDocumentDetail(value)
  };
  document!: DocumentDetailDto;
  preview: any;

  constructor(private  service: DocumentsListService, private san: DomSanitizer) {
  }

  getDocumentDetail(id: number){
      this.service.getDocument(id).pipe(
        tap((document) => {
          this.document = document;
          this.preview = this.san.bypassSecurityTrustResourceUrl(`data:application/pdf;base64,${document.content}`)
        })
      ).subscribe();    
  }
}