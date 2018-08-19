import { Component } from '@angular/core';
import { IResult } from '../../Interfaces/result'
import { QuizService } from '../../services/quiz.service'
import { ActivatedRoute, Router } from '@angular/router';
import { map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
    selector: 'app-result-editor',
    templateUrl: `result-editor.component.html`,
    styles: [``]
})
export class ResultEditorComponent {
    title = 'Edit';
    result: IResult | undefined;

    constructor(private quizService: QuizService, private route: ActivatedRoute, private router: Router) { }

    ngOnInit() {
        this.route.paramMap.pipe(map(params => {
                    return { quizId: +params.get('id')!, id: +params.get('rid')! }
                }),
                switchMap(ids => {
                    if (ids.id) {
                        return this.quizService.getResult(ids.quizId, ids.id);
                    } else {
                        this.title = "Create";
                        const q = { quizId: ids.quizId, id: ids.id } as IResult;
                        return of(q);
                    }
                }))
            .subscribe(data => this.result = data);
    }

    submit(result: IResult) {
        this.quizService.postOrUpdateResult(result)
            .subscribe(_ => {
                this.router.navigate(['/quiz', this.result!.quizId!]);
            }, error => console.log(error));
    }

    remove() {
        if (confirm('are you sure to delete?')) {
            this.quizService.removeResult(this.result!.quizId, this.result!.id)
                .subscribe(_ => this.router.navigate(['/quiz', this.result!.quizId]),
                    error => console.log(error));
        }
    }
}