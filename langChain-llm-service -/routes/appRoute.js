import express from "express";
import { retrievalFunction } from "../controllers/retrievalFunction.js";

const router = express.Router();

router.get('/retrieval', retrievalFunction);

export default router;
