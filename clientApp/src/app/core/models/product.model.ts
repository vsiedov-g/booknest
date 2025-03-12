
export class Product{
    constructor(
        public id: number, 
        public title: string,
        public description: string,
        public author: string,
        public categories: string[],
        public price: number, 
        public imageUrl: string,
    ){}
}