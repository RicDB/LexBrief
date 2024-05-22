import { Component, Input } from '@angular/core';
import {CommonModule} from "@angular/common";
import { DocumentDetailDto, DocumentsListService } from '../documents-list/documents-list.service';
import { tap } from 'rxjs';

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

  constructor(private  service: DocumentsListService) {
  }

  getDocumentDetail(id: number){
      this.service.getDocument(id).pipe(
        tap((document) => {
          this.document = document;
        })
      ).subscribe();    
  }
}
